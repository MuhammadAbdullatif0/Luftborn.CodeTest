export interface Product {
  id: number;
  name: string;
  description: string;
  price: number;
  pictureUrl: string;
  productCategoryId: number;
  categoryName?: string;
}

export interface CreateProduct {
  name: string;
  description: string;
  price: number;
  pictureUrl: string;
  productCategoryId: number;
}
