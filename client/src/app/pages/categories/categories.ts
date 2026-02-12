import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../core/services/api.service';
import { Category } from '../../core/models/category.model';

@Component({
  selector: 'app-categories',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './categories.html',
  styleUrl: './categories.scss',
})
export class Categories {
  private api = inject(ApiService);

  protected categories = signal<Category[]>([]);
  protected loading = signal(true);
  protected error = signal<string | null>(null);
  protected modalOpen = signal(false);
  protected editingId = signal<number | null>(null);
  protected formName = '';
  protected formSaving = signal(false);
  protected deleteConfirmId = signal<number | null>(null);

  constructor() {
    this.load();
  }

  protected load(): void {
    this.loading.set(true);
    this.error.set(null);
    this.api.getCategories().subscribe({
      next: (res) => {
        if (res.success && res.data) this.categories.set(res.data);
        else this.error.set(res.message ?? 'Failed to load');
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Failed to connect to API');
        this.loading.set(false);
      },
    });
  }

  protected openCreate(): void {
    this.editingId.set(null);
    this.formName = '';
    this.modalOpen.set(true);
  }

  protected openEdit(cat: Category): void {
    this.editingId.set(cat.id);
    this.formName = cat.name;
    this.modalOpen.set(true);
  }

  protected closeModal(): void {
    this.modalOpen.set(false);
    this.editingId.set(null);
    this.formName = '';
    this.deleteConfirmId.set(null);
  }

  protected save(): void {
    const name = this.formName.trim();
    if (!name) return;

    this.formSaving.set(true);
    const id = this.editingId();

    const req = id
      ? this.api.updateCategory(id, { name })
      : this.api.createCategory({ name });

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
    this.api.deleteCategory(id).subscribe({
      next: (res) => {
        if (res.success) {
          this.closeModal();
          this.load();
        } else this.error.set(res.message ?? 'Failed to delete');
      },
      error: () => this.error.set('Failed to delete'),
    });
  }
}
