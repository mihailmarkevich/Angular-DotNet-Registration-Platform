import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UsernameAvailabilityResponse } from '../models/username-availability.dto';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class UsersService {
  private readonly baseUrl = `${environment.apiBaseUrl}/api/registration`;

  constructor(private http: HttpClient) {}

  checkUsername(username: string): Observable<UsernameAvailabilityResponse> {
    const params = new HttpParams().set('username', username);
    return this.http.get<UsernameAvailabilityResponse>(`${this.baseUrl}/check-username`, { params });
  }
}
