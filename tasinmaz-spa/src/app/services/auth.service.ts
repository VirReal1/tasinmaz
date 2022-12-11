import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelper, tokenNotExpired } from 'angular2-jwt';
import { LoginKullanici } from '../models/loginKullanici';
import { RegisterKullanici } from '../models/registerKullanici';
import { AlertifyService } from './alertify.service';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private http: HttpClient, private router: Router, private alertifyService: AlertifyService) {}
  path = 'https://localhost:44343/api/users/';
  jwtHelper: JwtHelper = new JwtHelper();
  TOKEN_KEY = 'token';

  login(loginKullanici: LoginKullanici) {
    let headers = new HttpHeaders();
    headers = headers.append('Content-Type', 'application/json');
    this.http.post(this.path + 'login', loginKullanici, { headers: headers, responseType: 'text' }).subscribe((data) => {
      this.saveToken(data);
      this.alertifyService.success('Successfully logged in.');
      this.router.navigateByUrl('/tasinmazlar');
    });
  }

  register(registerKullanici: RegisterKullanici) {
    let headers = new HttpHeaders();
    headers = headers.append('Content-Type', 'application/json');
    this.http.post(this.path + 'register', registerKullanici, { headers: headers, responseType: 'text' }).subscribe((data) => {});
  }
  saveToken(token) {
    localStorage.setItem(this.TOKEN_KEY, token);
  }

  logout() {
    localStorage.removeItem(this.TOKEN_KEY);
    this.router.navigateByUrl('/login');
    this.alertifyService.success('Successfully logged out.');
  }

  loggedIn() {
    return tokenNotExpired(this.TOKEN_KEY);
  }

  get adminMi(){
    if (JSON.parse(localStorage.getItem('token')).adminMi === true) {
      return true;
    }
    else{
      return false;
    }
  };
}
