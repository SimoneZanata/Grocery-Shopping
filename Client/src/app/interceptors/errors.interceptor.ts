import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { catchError, Observable } from 'rxjs';
import { Router, NavigationExtras } from '@angular/router';

@Injectable()
export class ErrorsInterceptor implements HttpInterceptor {

  constructor(private router: Router) {}
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error) {
          switch (error.status) {
            case 401:
              localStorage.removeItem('user')
              this.router.navigateByUrl("/login");
              setTimeout(() => {
                alert("Errore durante l'autenticazione");
              }, 200);
              break;
            case 404:
              setTimeout(() => {
                alert("Pagina non trovata");
              }, 200);
              break;
            case 500:
              const navigationExtras: NavigationExtras = {state: {error: error.error}};
              this.router.navigateByUrl('/', navigationExtras);
              break;
          }
        }
        throw error;
      })
    )
  }
}