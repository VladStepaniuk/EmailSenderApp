import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InputsBlockComponent } from './inputs-block.component';

describe('InputsBlockComponent', () => {
  let component: InputsBlockComponent;
  let fixture: ComponentFixture<InputsBlockComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [InputsBlockComponent]
    });
    fixture = TestBed.createComponent(InputsBlockComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
