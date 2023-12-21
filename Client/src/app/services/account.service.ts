import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { User } from '../models/User';
import { LoginUser } from '../models/LoginUser';
import { RegisterUser } from '../models/RegisterUser';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  accountUrl = 'https://localhost:5001/account';

  constructor(private http: HttpClient) {}

  login(loginData: LoginUser) {
    return this.http.post<LoginUser>(`${this.accountUrl}/login`, loginData);
  }

  register(registerData: RegisterUser){
    return this.http.post(`${this.accountUrl}/register`, registerData);
  }

  logout() {
    localStorage.removeItem("user");
  }

  isAuthenticated() {
    return !!localStorage.getItem("user");
  }

  getCurrentUser() {
    const user = JSON.parse(localStorage.getItem("user") || '') as User;
    return user;
  }
}
