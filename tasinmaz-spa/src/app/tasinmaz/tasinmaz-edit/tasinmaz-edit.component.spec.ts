import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TasinmazEditComponent } from './tasinmaz-edit.component';

describe('TasinmazEditComponent', () => {
  let component: TasinmazEditComponent;
  let fixture: ComponentFixture<TasinmazEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TasinmazEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TasinmazEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
