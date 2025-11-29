import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  ValidatorFn,
  Validators,
  AsyncValidatorFn
} from '@angular/forms';
import { Observable, of } from 'rxjs';
import { debounceTime, distinctUntilChanged, filter, switchMap, map, catchError, finalize } from 'rxjs/operators';
import { MatStepperModule } from '@angular/material/stepper';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { IndustriesService } from '../../services/industries.service';
import { RegistrationService } from '../../services/registration.service';
import { UsersService } from '../../services/users.service';
import { CompaniesService } from '../../services/companies.service';
import { IndustryDto } from '../../models/industry.dto';
import { CompanySuggestionDto } from '../../models/company.dto';
import { RegistrationRequestDto, RegistrationResponseDto } from '../../models/registration.dto';
import { NotificationService } from 'src/app/core/services/notification.service';

@Component({
  selector: 'app-registration-wizard',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatStepperModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatCheckboxModule,
    MatButtonModule,
    MatIconModule,
    MatAutocompleteModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './registration-wizard.component.html',
  styleUrl: './registration-wizard.component.css'
})
export class RegistrationWizardComponent implements OnInit {
  companyForm!: FormGroup;
  userForm!: FormGroup;
  termsForm!: FormGroup;

  industries$: Observable<IndustryDto[]> | undefined;
  companySuggestions$: Observable<CompanySuggestionDto[]> | undefined;

  usernameCheckInProgress = false;
  isSubmitting = false;
  serverSuccess: string | null = null;
  lastResponse: RegistrationResponseDto | null = null;

  constructor(
    private readonly fb: FormBuilder,
    private readonly industriesService: IndustriesService,
    private readonly registrationService: RegistrationService,
    private readonly usersService: UsersService,
    private readonly companiesService: CompaniesService,
    private readonly notifications: NotificationService
  ) {}

  ngOnInit(): void {
    this.buildForms();
    this.loadIndustries();
    this.setupCompanyAutocomplete();
  }

  private buildForms(): void {
    this.companyForm = this.fb.group({
      companyName: [
        '', 
        [
          Validators.required, 
          Validators.maxLength(255),
          Validators.pattern(/^[^<>]*$/)
        ]
      ],
      industryId: [null, [Validators.required]]
    });

    this.userForm = this.fb.group(
      {
        firstName: [
          '', 
          [
            Validators.required, 
            Validators.maxLength(100),
            Validators.pattern(/^[^<>]*$/)
          ]
        ],
        lastName: [
          '', 
          [
            Validators.required, 
            Validators.maxLength(100),
            Validators.pattern(/^[^<>]*$/)
          ]
        ],
        userName: [
          '', 
          [
            Validators.required, 
            Validators.maxLength(100),
            Validators.pattern(/^[a-zA-Z0-9._-]+$/)
          ], 
          [this.usernameAvailableValidator()]],
        email: ['', [Validators.email, Validators.maxLength(256)]],
        password: ['', [Validators.required, Validators.minLength(8)]],
        passwordRepeat: ['', [Validators.required]]
      },
      {
        validators: [this.passwordsMatchValidator()]
      }
    );

    this.termsForm = this.fb.group({
      acceptTerms: [false, [Validators.requiredTrue]],
      acceptPrivacy: [false, [Validators.requiredTrue]]
    });
  }

  private setupCompanyAutocomplete(): void {
    const companyNameControl = this.companyForm.get('companyName');
    const industryIdControl = this.companyForm.get('industryId');

    if (!companyNameControl || !industryIdControl) {
      return;
    }

    this.companySuggestions$ = companyNameControl.valueChanges.pipe(
      map(value => {
        if (industryIdControl.disabled) {
          industryIdControl.enable({ emitEvent: false });
          industryIdControl.reset(null, { emitEvent: false });
        }

        return typeof value === 'string' ? value.trim() : '';
      }),
      debounceTime(300),
      distinctUntilChanged(),
      switchMap(value => {
        if (!value || value.length < 2) {
          return of<CompanySuggestionDto[]>([]);
        }

        return this.companiesService
          .searchCompanies(value, industryIdControl.value as number | null)
          .pipe(
            catchError(err => {
              console.error('Company search failed', err);

              if (err.status === 400 && err.error?.errors) {
                this.notifications.showError('Invalid company search request.');
              } else {
                this.notifications.showError(
                  'Could not load company suggestions. You can still type your company name manually.'
                );
              }

              return of<CompanySuggestionDto[]>([]);
            })
          );
      })
    );
  }


  private usernameAvailableValidator(): AsyncValidatorFn {
    return (control: AbstractControl): Observable<ValidationErrors | null> => {
      const value = (control.value ?? '').toString().trim();
      if (!value) {
        return of(null);
      }

      this.usernameCheckInProgress = true;

      return this.usersService.checkUsername(value).pipe(
        map(result => (result.isAvailable ? null : { usernameTaken: true })),
        catchError(err => {
          console.error('Username check failed', err);

          if (err.status === 400 && err.error?.errors) {
            this.notifications.showError('Invalid username check request.');
          } else {
            this.notifications.showError(
              'Could not verify username. It may still be available.'
            );
          }

          return of(null);
        }),
        finalize(() => {
          this.usernameCheckInProgress = false;
        })
      );
    };
  }


  private passwordsMatchValidator(): ValidatorFn {
    return (group: AbstractControl): ValidationErrors | null => {
      const passwordControl = group.get('password');
      const repeatControl = group.get('passwordRepeat');

      if (!passwordControl || !repeatControl) {
        return null;
      }

      const password = passwordControl.value;
      const repeat = repeatControl.value;

      if (!password || !repeat) {
        if (repeatControl.hasError('passwordsMismatch')) {
          const errors = { ...(repeatControl.errors ?? {}) };
          delete errors['passwordsMismatch'];
          repeatControl.setErrors(Object.keys(errors).length ? errors : null);
        }
        return null;
      }

      if (password === repeat) {
        if (repeatControl.hasError('passwordsMismatch')) {
          const errors = { ...(repeatControl.errors ?? {}) };
          delete errors['passwordsMismatch'];
          repeatControl.setErrors(Object.keys(errors).length ? errors : null);
        }
        return null;
      }

      const existingErrors = repeatControl.errors ?? {};
      repeatControl.setErrors({ ...existingErrors, passwordsMismatch: true });

      return { passwordsMismatch: true };
    };
  }


  get companyControls(): { [key: string]: AbstractControl } {
    return this.companyForm.controls;
  }

  get userControls(): { [key: string]: AbstractControl } {
    return this.userForm.controls;
  }

  get termsControls(): { [key: string]: AbstractControl } {
    return this.termsForm.controls;
  }

  onCompanyOptionSelected(option: CompanySuggestionDto): void {
    const companyNameControl = this.companyForm.get('companyName');
    const industryIdControl = this.companyForm.get('industryId');

    if (!companyNameControl || !industryIdControl) {
      return;
    }

    this.companyForm.patchValue(
      {
        companyName: option.name,
        industryId: option.industryId
      },
      { emitEvent: false }
    );

    industryIdControl.disable({ emitEvent: false });
  }

  canProceedFromCompany(): boolean {
    return this.companyForm.valid;
  }

  canProceedFromUser(): boolean {
    return this.userForm.valid;
  }

  canProceedFromTerms(): boolean {
    return this.termsForm.valid;
  }

  submitRegistration(): void {
    if (!this.companyForm.valid || !this.userForm.valid || !this.termsForm.valid) {
      this.companyForm.markAllAsTouched();
      this.userForm.markAllAsTouched();
      this.termsForm.markAllAsTouched();
      this.notifications.showError('Please fix the errors in the form before submitting.');
      return;
    }

    this.serverSuccess = null;
    this.lastResponse = null;
    this.isSubmitting = true;

    const payload: RegistrationRequestDto = {
      companyName: this.companyControls['companyName'].value,
      industryId: this.companyControls['industryId'].value,
      firstName: this.userControls['firstName'].value,
      lastName: this.userControls['lastName'].value,
      userName: this.userControls['userName'].value,
      email: this.userControls['email'].value || null,
      password: this.userControls['password'].value,
      passwordRepeat: this.userControls['passwordRepeat'].value,
      acceptTerms: this.termsControls['acceptTerms'].value,
      acceptPrivacy: this.termsControls['acceptPrivacy'].value
    };

    this.registrationService.register(payload).subscribe({
      next: (res: RegistrationResponseDto) => {
        this.isSubmitting = false;
        this.lastResponse = res;
        this.serverSuccess = `Registration completed. UserId=${res.userId}, CompanyId=${res.companyId}.`;

        this.notifications.showSuccess('Registration completed successfully.');
      },
      error: (err: any) => {
        this.isSubmitting = false;
        console.error('Registration failed', err);

        if (err.status === 400 && err.error?.errors) {
          const errorDict = err.error.errors as { [key: string]: string[] };
          const messages = Object.values(errorDict)
            .flat()
            .filter(m => !!m);

          const message =
            messages.length > 0
              ? messages.join(' ')
              : 'Validation error. Please review the inputs.';

          this.notifications.showError(message);
        } else if (err.status === 400 && err.error?.message) {
          this.notifications.showError(err.error.message);
        } else {
          this.notifications.showError('An unexpected error occurred while registering.');
        }
      }
    });
  }


  buildSummaryCompany(): string {
    const companyName = this.companyControls['companyName'].value;
    return companyName ? companyName : '-';
  }

  buildSummaryIndustryName(industries: IndustryDto[] | null): string {
    if (!industries) return '-';
    const id = this.companyControls['industryId'].value as number | null;
    const match = industries.find(i => i.id === id);
    return match?.name ?? '-';
  }

  buildSummaryUserFullName(): string {
    const first = this.userControls['firstName'].value;
    const last = this.userControls['lastName'].value;
    return [first, last].filter(Boolean).join(' ') || '-';
  }

  loadIndustries(): void {
    this.industries$ = this.industriesService.getIndustries().pipe(
      catchError(err => {
        console.error('Failed to load industries', err);

        if (err.status === 400 && err.error?.errors) {
          this.notifications.showError('Invalid industries request.');
        } else {
          this.notifications.showError('Failed to load industries. Please try again later.');
        }

        return of<IndustryDto[]>([]);
      })
    );
  }


}
