import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IndustryDto } from '../models/industry.dto';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class IndustriesService {
  private readonly baseUrl = `${environment.apiBaseUrl}/api/industries`;

  constructor(private http: HttpClient) {}

  getIndustries(): Observable<IndustryDto[]> {
    return this.http.get<IndustryDto[]>(this.baseUrl);
  }
}
