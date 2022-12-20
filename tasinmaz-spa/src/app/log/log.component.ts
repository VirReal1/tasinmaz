import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Log } from '../models/log';
import { AlertifyService } from '../services/alertify.service';
import { LogService } from '../services/log.service';

@Component({
  selector: 'app-log',
  templateUrl: './log.component.html',
  styleUrls: ['./log.component.css'],
  providers: [LogService],
})
export class LogComponent implements OnInit {
  constructor(private logService: LogService, private alertifyService: AlertifyService, private formBuilder: FormBuilder) {}

  loglar: Log[];
  searchForm: FormGroup;

  ngOnInit() {
    this.getAllLoglar();

    this.createSearchForm();
  }

  createSearchForm() {
    this.searchForm = this.formBuilder.group({ id: [], kullaniciIp: [], tarih: [], durum: [], islem: [], aciklama: [], kullaniciId: [] });
  }

  getAllLoglar() {
    this.logService.getLoglar().subscribe((data) => {
      if (data['warning']) {
        this.alertifyService.warning(data['message']);
      } else if (data['error']) {
        this.alertifyService.error(data['message']);
      }
      this.loglar = data['data'];
    });
  }

  searchLoglar() {
    if (this.searchForm.valid) {
      let searchParameters: Log;
      searchParameters = Object.assign({}, this.searchForm.value);
      this.logService.getLoglarBySearch(searchParameters).subscribe((data) => {
        if (data['error']) {
          this.alertifyService.error(data['message']);
          this.loglar = null;
        } else if (data['warning']) {
          this.alertifyService.warning(data['message']);
          this.loglar = null;
        } else {
          this.alertifyService.success(data['message']);
          this.loglar = data['data'];
        }
      });
    }
  }
}
