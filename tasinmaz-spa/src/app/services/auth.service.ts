import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelper, tokenNotExpired } from 'angular2-jwt';
import { LoginKullanici } from '../models/loginKullanici';
import { AlertifyService } from './alertify.service';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private jwtHelper: JwtHelper, private http: HttpClient, private router: Router, private alertifyService: AlertifyService) {}
  private path = 'https://localhost:44343/api/users/';
  private TOKEN_KEY = 'token';
  private ADMIN_KEY = 'adminMi';
  private KULLANICIID_KEY = 'kullaniciId';

  login(loginKullanici: LoginKullanici) {
    this.http.post(this.path + 'login', loginKullanici).subscribe((data) => {
      if (data['warning']) {
        this.alertifyService.warning(data['message']);
      } else if (data['error']) {
        this.alertifyService.error(data['message']);
      } else {
        localStorage.setItem(this.ADMIN_KEY, data['data'].adminMi);
        localStorage.setItem(this.KULLANICIID_KEY, data['data'].id);
        localStorage.setItem(this.TOKEN_KEY, data['data'].token);
        console.log(this.jwtHelper.decodeToken(data['data'].token.toString()));
        this.alertifyService.success(data['message']);
        this.router.navigateByUrl('/tasinmazlar');
      }
    });
  }

  logout() {
    localStorage.removeItem(this.ADMIN_KEY);
    localStorage.removeItem(this.KULLANICIID_KEY);
    localStorage.removeItem(this.TOKEN_KEY);
    this.router.navigateByUrl('/login');
    this.alertifyService.success('Çıkış başarılı.');
  }

  loggedIn() {
    return tokenNotExpired(this.TOKEN_KEY);
  }

  get adminMi(): boolean {
    return JSON.parse(localStorage.getItem(this.ADMIN_KEY));
  }

  get kullaniciId() {
    return JSON.parse(localStorage.getItem(this.KULLANICIID_KEY));
  }
}
