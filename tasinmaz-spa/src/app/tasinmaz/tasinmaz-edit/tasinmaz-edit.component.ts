import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Tasinmaz } from 'src/app/models/tasinmaz';
import { AuthService } from 'src/app/services/auth.service';
import Map from 'ol/Map';
import View from 'ol/View';
import TileLayer from 'ol/layer/Tile';
import OSM from 'ol/source/OSM';
import { Il } from 'src/app/models/il';
import { Ilce } from 'src/app/models/ilce';
import { Mahalle } from 'src/app/models/mahalle';
import { LocationService } from 'src/app/services/location.service';
import { AlertifyService } from 'src/app/services/alertify.service';

@Component({
  selector: 'app-tasinmaz-edit',
  templateUrl: './tasinmaz-edit.component.html',
  styleUrls: ['./tasinmaz-edit.component.css'],
  providers: [AuthService, LocationService],
})
export class TasinmazEditComponent implements OnInit {
  constructor(private authService: AuthService, private locationService: LocationService, private alertifyService: AlertifyService, private formBuilder: FormBuilder) {}

  @Input() editParameters: Tasinmaz;
  @Output() toTasinmazPage = new EventEmitter();
  @Output() goBack = new EventEmitter();

  editForm: FormGroup;
  buttonAdi: string;
  map: Map;
  iller: Il[];
  ilceler: Ilce[];
  mahalleler: Mahalle[];

  ngOnInit() {
    if (this.editParameters != null) {
      this.createUpdateForm();
    } else {
      this.createAddForm();
    }
    this.getIller();
    this.createMap();
  }

  createAddForm() {
    this.editForm = this.formBuilder.group({
      adi: [],
      ilAdi: [],
      ilceAdi: [],
      mahalleAdi: [],
      ada: [],
      parsel: [],
      nitelik: [],
      koordinatBilgileri: [],
    });
    this.buttonAdi = 'Taşınmaz Ekle';
  }

  createUpdateForm() {
    this.editForm = this.formBuilder.group({
      adi: [this.editParameters.adi],
      ilAdi: [this.editParameters.ilAdi],
      ilceAdi: [this.editParameters.ilceAdi],
      mahalleAdi: [this.editParameters.mahalleAdi],
      ada: [this.editParameters.ada],
      parsel: [this.editParameters.parsel],
      nitelik: [this.editParameters.nitelik],
      koordinatBilgileri: [this.editParameters.koordinatBilgileri],
    });
    this.buttonAdi = 'Taşınmazı Güncelle';
  }

  createMap() {
    this.map = new Map({
      view: new View({
        center: [0, 0],
        zoom: 1,
      }),
      layers: [
        new TileLayer({
          source: new OSM(),
        }),
      ],
      target: 'ol-map',
    });
  }

  editTasinmaz() {
    if (this.editForm.valid) {
      let editTasinmazData: Tasinmaz = null;
      editTasinmazData = Object.assign({}, this.editForm.value);
      editTasinmazData.kullaniciId = this.authService.kullaniciId;
      if (this.editParameters !== null) {
        editTasinmazData.id = this.editParameters.id;
      } else {
        editTasinmazData.id = 0;
      }

      this.toTasinmazPage.emit(editTasinmazData);
    }
  }

  getIller() {
    this.locationService.getIller().subscribe((data) => {
      if (data['warning']) {
        this.alertifyService.warning(data['message']);
      } else if (data['error']) {
        this.alertifyService.error(data['message']);
      }
      this.iller = data['data'];
      this.mahalleler = null;
    });
  }

  getIlceler() {
    this.locationService.getIlceler(this.editForm.controls.ilAdi.value).subscribe((data) => {
      if (data['warning']) {
        this.alertifyService.warning(data['message']);
      } else if (data['error']) {
        this.alertifyService.error(data['message']);
      }
      this.ilceler = data['data'];
    });
  }

  getMahalleler() {
    console.log(this.editForm.controls.ilceAdi.value);
    this.locationService.getMahalleler(this.editForm.controls.ilceAdi.value).subscribe((data) => {
      if (data['warning']) {
        this.alertifyService.warning(data['message']);
      } else if (data['error']) {
        this.alertifyService.error(data['message']);
      }
      this.mahalleler = data['data'];
    });
  }

  goBackButton() {
    this.goBack.emit();
  }
}
