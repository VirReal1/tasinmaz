/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { UserOpComponent } from './userOp.component';

describe('UserOpComponent', () => {
  let component: UserOpComponent;
  let fixture: ComponentFixture<UserOpComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UserOpComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserOpComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
