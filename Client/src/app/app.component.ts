import { Component } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { RegistrationWizardComponent } from './features/registration-wizard/registration-wizard.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    MatToolbarModule, 
    MatCardModule,
    MatSnackBarModule,
    RegistrationWizardComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'Registration Platform';
}
