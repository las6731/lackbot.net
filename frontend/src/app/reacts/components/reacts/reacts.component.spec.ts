import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReactsComponent } from './reacts.component';

describe('ReactsComponent', () => {
  let component: ReactsComponent;
  let fixture: ComponentFixture<ReactsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReactsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReactsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
