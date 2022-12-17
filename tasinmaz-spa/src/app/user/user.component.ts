import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { FormGroup } from '@angular/forms';
import { AddUpdateKullanici } from '../models/addUpdateKullanici';
import { ShowDeleteKullanici } from '../models/showDeleteKullanici';
import { AlertifyService } from '../services/alertify.service';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css'],
  providers: [UserService],
})
export class UserComponent implements OnInit {
  constructor(private userService: UserService, private alertifyService: AlertifyService, private formBuilder: FormBuilder) {}

  kullanicilar: ShowDeleteKullanici[];
  searchForm: FormGroup;
  ngOnInit() {
    this.createSearchForm();

    this.getAllKullanicilar();
  }

  createSearchForm() {
    this.searchForm = this.formBuilder.group({ id: [], ad: [], soyad: [], email: [], searchAdminMi: [] });
  }

  getAllKullanicilar() {
    this.userService.getKullanicilar().subscribe((data) => {
      if (data['warning']) {
        this.alertifyService.warning(data['message']);
      } else if (data['error']) {
        this.alertifyService.error(data['message']);
      }
      this.kullanicilar = data['data'];
    });
  }

  searchKullanicilar() {
    if (this.searchForm.valid) {
      let searchParameters: ShowDeleteKullanici;
      if (this.searchForm.value.id === null) {
        this.searchForm.value.id = 0;
      }

      searchParameters = Object.assign({}, this.searchForm.value);
      if (searchParameters.searchAdminMi === undefined) {
        searchParameters.searchAdminMi = null;
      }
      this.userService.getKullanicilarBySearch(searchParameters).subscribe((data) => {
        if (data['error']) {
          this.alertifyService.error(data['message']);
          this.kullanicilar = null;
        } else if (data['warning']) {
          this.alertifyService.warning(data['message']);
          this.kullanicilar = null;
        } else {
          this.alertifyService.success(data['message']);
          this.kullanicilar = data['data'];
        }
      });
    }
  }

  kullaniciEditData: ShowDeleteKullanici;
  editPressed: boolean = false;
  editKullanici(kullanici) {
    this.kullaniciEditData = kullanici;
    this.editPressed = true;
  }

  addKullanici(kullanici) {
    this.userService.addKullanici(kullanici).subscribe((data) => {
      if (data['error']) {
        this.alertifyService.error(data['message']);
      } else if (data['warning']) {
        this.alertifyService.warning(data['message']);
      } else {
        this.alertifyService.success(data['message']);
        this.getAllKullanicilar();
      }
    });
  }

  updateKullanici(kullanici) {
    this.userService.updateKullanici(kullanici).subscribe((data) => {
      if (data['error']) {
        this.alertifyService.error(data['message']);
      } else if (data['warning']) {
        this.alertifyService.warning(data['message']);
      } else {
        this.alertifyService.success(data['message']);
        this.getAllKullanicilar();
      }
    });
  }

  deleteKullanici(kullanici) {
    this.userService.deleteKullanici(kullanici).subscribe((data) => {
      if (data['error']) {
        this.alertifyService.error(data['message']);
      } else if (data['warning']) {
        this.alertifyService.warning(data['message']);
      } else {
        this.alertifyService.success(data['message']);
        this.getAllKullanicilar();
      }
    });
  }

  toKullaniciPage(kullanici: AddUpdateKullanici) {
    if (kullanici !== null) {
      if (kullanici.id === 0) {
        this.addKullanici(kullanici);
      } else {
        this.updateKullanici(kullanici);
      }
    }

    this.editPressed = false;
  }

  goBack() {
    this.editPressed = false;
  }
}
