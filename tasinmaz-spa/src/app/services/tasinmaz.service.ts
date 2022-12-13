import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { Tasinmaz } from '../models/tasinmaz';
import { AlertifyService } from './alertify.service';

@Injectable()
export class TasinmazService {
  constructor(private http: HttpClient, private router: Router, private alertifyService: AlertifyService) {}
  path = 'https://localhost:44343/api/tasinmazlar/';

  getTasinmazlar(): Observable<Tasinmaz[]> {
    return this.http.get<Tasinmaz[]>(this.path);
  }

  getTasinmazlarBySearch(tasinmaz) {
    this.http.post(this.path + 'search', tasinmaz).subscribe((data) => {
      (response: Response) => {
        if (response.status == 404) {
          this.alertifyService.error(response.statusText);
          return null;
        } else if (response.status == 500) {
          this.alertifyService.error(response.statusText);
          return null;
        }
        this.alertifyService.success('Taşınmazlar başarıyla arandı.');
        return data;
      };
    });
  }

  addTasinmaz(tasinmaz) {
    this.http.post(this.path, tasinmaz).subscribe((response: Response) => {
      if (response.status == 400) {
        this.alertifyService.error(response.statusText);
      } else if (response.status == 500) {
        this.alertifyService.error(response.statusText);
      } else {
        this.alertifyService.success('Taşınmaz başarıyla eklendi.');
        this.router.navigateByUrl('tasinmazlar');
      }
    });
  }

  updateTasinmaz(tasinmaz) {
    this.http.put(this.path, tasinmaz).subscribe((response: Response) => {
      if (response.status == 500) {
        this.alertifyService.error(response.statusText);
      } else {
        this.alertifyService.success('Taşınmaz başarıyla güncellendi.');
        this.router.navigateByUrl('tasinmazlar');
      }
    });
  }

  deleteTasinmaz(tasinmaz) {
    const options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      body: tasinmaz,
    };
    this.http.delete(this.path, options).subscribe((response: any) => {
      if (response.status == 500) {
        this.alertifyService.error(response.statusText);
      } else {
        this.alertifyService.success('Taşınmaz başarıyla silindi.');
        this.router.navigateByUrl('tasinmazlar');
      }
    });
  }
}
