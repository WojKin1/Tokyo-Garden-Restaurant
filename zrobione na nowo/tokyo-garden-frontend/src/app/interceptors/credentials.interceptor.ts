import { HttpInterceptorFn } from '@angular/common/http';
import { environment } from '../../environments/environment';

// Wysyłaj cookies tylko do backendu (URL zaczyna się od environment.apiUrl)
export const credentialsInterceptor: HttpInterceptorFn = (req, next) => {
  if (req.url.startsWith(environment.apiUrl)) {
    req = req.clone({ withCredentials: true });
  }
  return next(req);
};
