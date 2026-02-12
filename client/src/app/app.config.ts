import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { idempotencyInterceptor } from './core/interceptors/idempotency.interceptor';
import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    provideHttpClient(withInterceptors([idempotencyInterceptor]))
  ]
};
