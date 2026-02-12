import { HttpInterceptorFn } from '@angular/common/http';

const MODIFYING_METHODS = ['POST', 'PUT', 'PATCH'];

export const idempotencyInterceptor: HttpInterceptorFn = (req, next) => {
  if (!MODIFYING_METHODS.includes(req.method) || req.headers.has('Idempotency-Key')) {
    return next(req);
  }

  const cloned = req.clone({
    headers: req.headers.set('Idempotency-Key', crypto.randomUUID()),
  });

  return next(cloned);
};
