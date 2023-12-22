import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginUser } from '../models/LoginUser';
import { RegisterUser } from '../models/RegisterUser';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  accountUrl = 'https://localhost:5001/account';
  constructor(private http: HttpClient) { }

  login(loginData: LoginUser) {
    return this.http.post<LoginUser>(`${this.accountUrl}/login`, loginData);
  }

  register(registerData: RegisterUser) {
    return this.http.post(`${this.accountUrl}/register`, registerData);
  }

  logout() {
    localStorage.removeItem("user");
  }

  isAuthenticated() {
    return !!localStorage.getItem("user");
  }

  getUserId(): number {
    const userString = localStorage.getItem("user");
    if (!userString) {
      return 0;
    }
    const user = JSON.parse(userString);
    return user.id;
  }
}
