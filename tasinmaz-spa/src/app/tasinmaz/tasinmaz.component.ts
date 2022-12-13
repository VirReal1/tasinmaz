import { Component, OnInit } from '@angular/core';
import { Tasinmaz } from '../models/tasinmaz';
import { TasinmazService } from '../services/tasinmaz.service';

@Component({
  selector: 'app-tasinmaz',
  templateUrl: './tasinmaz.component.html',
  styleUrls: ['./tasinmaz.component.css'],
  providers: [TasinmazService]
})
export class TasinmazComponent implements OnInit {

  constructor(private tasinmazService: TasinmazService) { }

  tasinmazlar: Tasinmaz[];

  ngOnInit() {
    this.tasinmazService.getTasinmazlar().subscribe((data)=>{
      this.tasinmazlar = data;
    });
  }

  deleteTasinmaz(tasinmaz){
    this.tasinmazService.deleteTasinmaz(tasinmaz);
  }

}
