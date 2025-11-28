import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RegistrationRequestDto, RegistrationResponseDto } from '../models/registration.dto';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class RegistrationService {
  private readonly baseUrl = `${environment.apiBaseUrl}/api/registration`;

  constructor(private http: HttpClient) {}

  register(request: RegistrationRequestDto): Observable<RegistrationResponseDto> {
    return this.http.post<RegistrationResponseDto>(this.baseUrl, request);
  }
}
