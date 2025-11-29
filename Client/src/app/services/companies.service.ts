import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { CompanySuggestionDto } from '../models/company.dto';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class CompaniesService {

  private readonly baseUrl = `${environment.apiBaseUrl}/api/companies`;

  constructor(private http: HttpClient) {}

  searchCompanies(term: string, industryId?: number | null): Observable<CompanySuggestionDto[]> {
    const query = term?.trim();
    if (!query || query.length < 2) {
      return of([]);
    }

    let params = new HttpParams().set('query', query);
    if (industryId && industryId > 0) {
      params = params.set('industryId', industryId.toString());
    }

    return this.http
      .get<CompanySuggestionDto[]>(`${this.baseUrl}/search`, { params })
      .pipe(
        catchError(err => {
          console.error('Company search failed', err);
          return of([]);
        })
      );
  }
}
