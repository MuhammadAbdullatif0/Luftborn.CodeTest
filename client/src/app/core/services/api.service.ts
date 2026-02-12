import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResult } from '../models/api-result.model';
import { Category, CreateCategory } from '../models/category.model';
import { CreateProduct, Product } from '../models/product.model';

@Injectable({ providedIn: 'root' })
export class ApiService {
  private readonly baseUrl = '/api';

  constructor(private http: HttpClient) {}

  getCategories(): Observable<ApiResult<Category[]>> {
    return this.http.get<ApiResult<Category[]>>(`${this.baseUrl}/categories`);
  }

  getCategory(id: number): Observable<ApiResult<Category>> {
    return this.http.get<ApiResult<Category>>(`${this.baseUrl}/categories/${id}`);
  }

  createCategory(dto: CreateCategory): Observable<ApiResult<Category>> {
    return this.http.post<ApiResult<Category>>(`${this.baseUrl}/categories`, dto);
  }

  updateCategory(id: number, dto: CreateCategory): Observable<ApiResult<Category>> {
    return this.http.put<ApiResult<Category>>(`${this.baseUrl}/categories/${id}`, dto);
  }

  deleteCategory(id: number): Observable<ApiResult<null>> {
    return this.http.delete<ApiResult<null>>(`${this.baseUrl}/categories/${id}`);
  }

  getProducts(pageIndex = 1, pageSize = 10, categoryIds?: string): Observable<ApiResult<Product[]>> {
    let url = `${this.baseUrl}/products?pageIndex=${pageIndex}&pageSize=${pageSize}`;
    if (categoryIds) url += `&categoryIds=${categoryIds}`;
    return this.http.get<ApiResult<Product[]>>(url);
  }

  getProduct(id: number): Observable<ApiResult<Product>> {
    return this.http.get<ApiResult<Product>>(`${this.baseUrl}/products/${id}`);
  }

  createProduct(dto: CreateProduct): Observable<ApiResult<Product>> {
    return this.http.post<ApiResult<Product>>(`${this.baseUrl}/products`, dto);
  }

  updateProduct(id: number, dto: CreateProduct): Observable<ApiResult<Product>> {
    return this.http.put<ApiResult<Product>>(`${this.baseUrl}/products/${id}`, dto);
  }

  deleteProduct(id: number): Observable<ApiResult<null>> {
    return this.http.delete<ApiResult<null>>(`${this.baseUrl}/products/${id}`);
  }
}
