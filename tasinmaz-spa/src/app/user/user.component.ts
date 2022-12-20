import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { FormGroup } from '@angular/forms';
import { AddKullanici } from '../models/addKullanici';
import { ShowKullanici } from '../models/showKullanici';
import { UpdateKullanici } from '../models/updateKullanici';
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

  kullanicilar: ShowKullanici[];
  searchForm: FormGroup;
  ngOnInit() {
    this.createSearchForm();

    this.getAllKullanicilar();
  }

  createSearchForm() {
    this.searchForm = this.formBuilder.group({ id: [], ad: [], soyad: [], email: [], userRole: [] });
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
      let searchParameters: ShowKullanici;
      if (this.searchForm.value.id === null) {
        this.searchForm.value.id = 0;
      }

      searchParameters = Object.assign({}, this.searchForm.value);

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

  kullaniciEditData: ShowKullanici;
  editPressed: boolean = false;
  editKullanici(kullanici) {
    this.kullaniciEditData = kullanici;
    this.editPressed = true;
  }

  addKullanici(kullanici: AddKullanici) {
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

  updateKullanici(kullanici: UpdateKullanici) {
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

  deleteKullanici(kullanici: ShowKullanici) {
    this.userService.deleteKullanici(kullanici.id).subscribe((data) => {
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

  toKullaniciPage(kullanici) {
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
