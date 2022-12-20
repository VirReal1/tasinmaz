import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { FormGroup } from '@angular/forms';
import { UpdateKullanici } from 'src/app/models/updateKullanici';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css'],
})
export class UserEditComponent implements OnInit {
  constructor(private authService: AuthService, private formBuilder: FormBuilder) {}

  @Input() editParameters: UpdateKullanici;
  @Output() toKullaniciPage = new EventEmitter();
  @Output() goBack = new EventEmitter();

  editForm: FormGroup;
  buttonAdi: string;

  ngOnInit() {
    if (this.editParameters != null) {
      this.createUpdateForm();
    } else {
      this.createAddForm();
    }
  }

  createAddForm() {
    this.editForm = this.formBuilder.group(
      {
        ad: [],
        soyad: [],
        email: [],
        password: [],
        confirmPassword: [],
        userRole: [,Validators.required],
      },
      {
        validator: this.passwordMatchValidator,
      }
    );
    this.buttonAdi = 'Kullanıcı Ekle';
  }

  passwordMatchValidator(g: FormGroup) {
    return g.get('password').value === g.get('confirmPassword').value ? null : { missMatch: true };
  }

  createUpdateForm() {
    this.editForm = this.formBuilder.group(
      {
        ad: [this.editParameters.ad],
        soyad: [this.editParameters.soyad],
        email: [this.editParameters.email],
        password: [],
        confirmPassword: [],
        userRole: [this.editParameters.userRole],
      },
      { validator: this.passwordMatchValidator }
    );
    this.buttonAdi = 'Kullanıcıyı Güncelle';
  }

  editKullanici() {
    if (this.editForm.valid) {
      let editKullaniciData: UpdateKullanici = null;
      editKullaniciData = Object.assign({}, this.editForm.value);
      editKullaniciData.logKullaniciId = this.authService.kullaniciId;
      if (this.editParameters !== null) {
        editKullaniciData.id = this.editParameters.id;
      } else {
        editKullaniciData.id = 0;
      }

      this.toKullaniciPage.emit(editKullaniciData);
    }
  }

  goBackButton() {
    this.goBack.emit();
  }
}
