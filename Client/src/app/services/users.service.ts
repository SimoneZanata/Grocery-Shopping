import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Item } from '../models/Item';

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  usersUrl = 'https://localhost:5001/users';

  constructor(private http: HttpClient) { }
  getItemfromUser(userId: number, itemId: number) {
    return this.http.get<Item>(`${this.usersUrl}/${userId}/items/${itemId}`);
  }

  getItemsfromUser(userId: number) {
    return this.http.get<Item[]>(`${this.usersUrl}/${userId}/items`);
  }

  addItemForUser(userId: number, item: Item) {
    return this.http.post(`${this.usersUrl}/${userId}/items/`, item);
  }

  editItemForUser(userId: number, itemId: number, item: Item) {
    return this.http.put(`${this.usersUrl}/${userId}/items/${itemId}`, item);
  }

  deleteItemfromUser(userId: number, itemId: number) {
    return this.http.delete(`${this.usersUrl}/${userId}/items/${itemId}`);
  }


}
