import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { of, Observable } from 'rxjs';
import { IndustryDto } from '../models/industry.dto';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class IndustriesService {
  private readonly baseUrl = `${environment.apiBaseUrl}/api/industries`;

  constructor(private http: HttpClient) {}

  getIndustries(): Observable<IndustryDto[]> {
    //return this.http.get<IndustryDto[]>(this.baseUrl);

    // Dummy data while backend is offline
  const dummy: IndustryDto[] = [
    { id: 1, name: 'Manufacturing' },
    { id: 2, name: 'IT Services' },
    { id: 3, name: 'Healthcare' },
    { id: 4, name: 'Finance' },
    { id: 5, name: 'Retail' }
  ];

  return of(dummy);

  }
}
