export interface Product {
    id: number;
    name: string;
    price: number;
    description: string;
    imageUrl?: string;
    quantity?: number;
    createdAt?: Date;
    updatedAt?: Date;
} 