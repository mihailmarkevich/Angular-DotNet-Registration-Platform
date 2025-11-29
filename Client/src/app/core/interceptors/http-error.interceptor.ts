import { Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpInterceptor,
  HttpHandler,
  HttpRequest,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { NotificationService } from '../services/notification.service';

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {

  constructor(private notifications: NotificationService) {}

  intercept(req: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 0) {
          this.notifications.showError(
            'Cannot reach the server. Please check your connection or try again later.'
          );
        }
        else if (error.status >= 500) {
          this.notifications.showError(
            'The server encountered an error. Please try again later.'
          );
        }

        return throwError(() => error);
      })
    );
  }
}
