import { DecimalPipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../core/services/api.service';
import { Category } from '../../core/models/category.model';
import { Product } from '../../core/models/product.model';

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [FormsModule, DecimalPipe],
  templateUrl: './products.html',
  styleUrl: './products.scss',
})
export class Products {
  private api = inject(ApiService);

  protected products = signal<Product[]>([]);
  protected categories = signal<Category[]>([]);
  protected loading = signal(true);
  protected error = signal<string | null>(null);
  protected modalOpen = signal(false);
  protected editingId = signal<number | null>(null);
  protected formSaving = signal(false);
  protected deleteConfirmId = signal<number | null>(null);

  protected form = {
    name: '',
    description: '',
    price: 0,
    pictureUrl: '',
    productCategoryId: 0,
  };

  protected categoryFilter = '';
  protected pageIndex = signal(1);
  protected pageSize = 10;

  constructor() {
    this.loadCategories();
    this.load();
  }

  protected loadCategories(): void {
    this.api.getCategories().subscribe({
      next: (res) => {
        if (res.success && res.data) this.categories.set(res.data);
      },
    });
  }

  protected load(): void {
    this.loading.set(true);
    this.error.set(null);
    const catIds = this.categoryFilter || undefined;
    this.api.getProducts(this.pageIndex(), this.pageSize, catIds).subscribe({
      next: (res) => {
        if (res.success && res.data) this.products.set(res.data);
        else this.error.set(res.message ?? 'Failed to load');
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Failed to connect to API');
        this.loading.set(false);
      },
    });
  }

  protected onCategoryFilterChange(): void {
    this.pageIndex.set(1);
    this.load();
  }

  protected setCategoryFilter(value: string): void {
    this.categoryFilter = value;
    this.onCategoryFilterChange();
  }

  protected openCreate(): void {
    this.editingId.set(null);
    this.resetForm();
    this.modalOpen.set(true);
  }

  protected openEdit(p: Product): void {
    this.editingId.set(p.id);
    this.form = {
      name: p.name,
      description: p.description,
      price: p.price,
      pictureUrl: p.pictureUrl,
      productCategoryId: p.productCategoryId,
    };
    this.modalOpen.set(true);
  }

  protected closeModal(): void {
    this.modalOpen.set(false);
    this.editingId.set(null);
    this.resetForm();
    this.deleteConfirmId.set(null);
  }

  private resetForm(): void {
    const cats = this.categories();
    this.form = {
      name: '',
      description: '',
      price: 0,
      pictureUrl: '',
      productCategoryId: cats.length ? cats[0].id : 0,
    };
  }

  protected save(): void {
    const f = this.form;
    if (!f.name.trim()) return;

    this.formSaving.set(true);
    const id = this.editingId();
    const dto = {
      name: f.name.trim(),
      description: f.description.trim(),
      price: f.price,
      pictureUrl: f.pictureUrl.trim() || '/images/placeholder.png',
      productCategoryId: f.productCategoryId,
    };

    const req = id
      ? this.api.updateProduct(id, dto)
      : this.api.createProduct(dto);

    req.subscribe({
      next: (res) => {
        if (res.success) {
          this.closeModal();
          this.load();
        } else this.error.set(res.message ?? 'Failed to save');
        this.formSaving.set(false);
      },
      error: () => {
        this.error.set('Failed to save');
        this.formSaving.set(false);
      },
    });
  }

  protected confirmDelete(id: number): void {
    this.deleteConfirmId.set(id);
  }

  protected cancelDelete(): void {
    this.deleteConfirmId.set(null);
  }

  protected delete(id: number): void {
    this.api.deleteProduct(id).subscribe({
      next: (res) => {
        if (res.success) {
          this.closeModal();
          this.load();
        } else this.error.set(res.message ?? 'Failed to delete');
      },
      error: () => this.error.set('Failed to delete'),
    });
  }

  protected getCategoryName(id: number): string {
    return this.categories().find((c) => c.id === id)?.name ?? '-';
  }
}
