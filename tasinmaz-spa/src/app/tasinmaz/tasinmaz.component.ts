import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Policies } from '../http/policies';
import { Tasinmaz } from '../models/tasinmaz';
import { AlertifyService } from '../services/alertify.service';
import { AuthService } from '../services/auth.service';
import { TasinmazService } from '../services/tasinmaz.service';

@Component({
  selector: 'app-tasinmaz',
  templateUrl: './tasinmaz.component.html',
  styleUrls: ['./tasinmaz.component.css'],
  providers: [TasinmazService, AuthService],
})
export class TasinmazComponent implements OnInit {
  constructor(private tasinmazService: TasinmazService, private authService: AuthService, private alertifyService: AlertifyService, private formBuilder: FormBuilder) {}

  tasinmazlar: Tasinmaz[];
  searchForm: FormGroup;

  ngOnInit() {
    this.createSearchForm();

    this.getAllTasinmazlar();
  }

  createSearchForm() {
    if (this.adminMi()) {
      this.searchForm = this.formBuilder.group({ id: [], adi: [], ilAdi: [], ilceAdi: [], mahalleAdi: [], ada: [], parsel: [], nitelik: [], koordinatBilgileri: [], kullaniciId: [] });
    } else {
      this.searchForm = this.formBuilder.group({ id: [], adi: [], ilAdi: [], ilceAdi: [], mahalleAdi: [], ada: [], parsel: [], nitelik: [], koordinatBilgileri: [] });
    }
  }

  getAllTasinmazlar() {
    this.tasinmazService.getTasinmazlar().subscribe((data) => {
      if (data['warning']) {
        this.alertifyService.warning(data['message']);
      } else if (data['error']) {
        this.alertifyService.error(data['message']);
      }
      this.tasinmazlar = data['data'];
    });
  }

  searchTasinmazlar() {
    if (this.searchForm.valid) {
      let searchParameters: Tasinmaz;
      if (this.searchForm.value.id === null) {
        this.searchForm.value.id = 0;
      }
      if (this.searchForm.value.kullaniciId === null) {
        this.searchForm.value.kullaniciId = 0;
      }

      searchParameters = Object.assign({}, this.searchForm.value);
      this.tasinmazService.getTasinmazlarBySearch(searchParameters).subscribe((data) => {
        if (data['error']) {
          this.alertifyService.error(data['message']);
          this.tasinmazlar = null;
        } else if (data['warning']) {
          this.alertifyService.warning(data['message']);
          this.tasinmazlar = null;
        } else {
          this.alertifyService.success(data['message']);
          this.tasinmazlar = data['data'];
        }
      });
    }
  }

  editTasinmaz(tasinmaz) {
    this.tasinmazEditData = tasinmaz;
    this.editPressed = true;
  }

  tasinmazEditData: Tasinmaz;

  editPressed: boolean = false;

  addTasinmaz(tasinmaz) {
    this.tasinmazService.addTasinmaz(tasinmaz).subscribe((data) => {
      if (data['error']) {
        this.alertifyService.error(data['message']);
      } else if (data['warning']) {
        this.alertifyService.warning(data['message']);
      } else {
        this.alertifyService.success(data['message']);
        this.getAllTasinmazlar();
      }
    });
  }

  updateTasinmaz(tasinmaz) {
    this.tasinmazService.updateTasinmaz(tasinmaz).subscribe((data) => {
      if (data['error']) {
        this.alertifyService.error(data['message']);
      } else if (data['warning']) {
        this.alertifyService.warning(data['message']);
      } else {
        this.alertifyService.success(data['message']);
        this.getAllTasinmazlar();
      }
    });
  }

  deleteTasinmaz(tasinmaz) {
    this.tasinmazService.deleteTasinmaz(tasinmaz.id).subscribe((data) => {
      if (data['error']) {
        this.alertifyService.error(data['message']);
      } else if (data['warning']) {
        this.alertifyService.warning(data['message']);
      } else {
        this.alertifyService.success(data['message']);
        this.getAllTasinmazlar();
      }
    });
  }

  toTasinmazPage(tasinmaz: Tasinmaz) {
    if (tasinmaz !== null) {
      if (tasinmaz.id === 0) {
        this.addTasinmaz(tasinmaz);
      } else {
        this.updateTasinmaz(tasinmaz);
      }
    }

    this.editPressed = false;
  }

  goBack() {
    this.editPressed = false;
  }

  adminMi(): boolean {
    if (this.authService.userRole == Policies.AdminPolicy) {
      return true;
    }
    return false;
  }
}
