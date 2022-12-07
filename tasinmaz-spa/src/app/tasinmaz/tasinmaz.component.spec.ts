/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { TasinmazComponent } from './tasinmaz.component';

describe('TasinmazComponent', () => {
  let component: TasinmazComponent;
  let fixture: ComponentFixture<TasinmazComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TasinmazComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TasinmazComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
