import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReactDialogComponent } from './react-dialog.component';

describe('ReactDialogComponent', () => {
  let component: ReactDialogComponent;
  let fixture: ComponentFixture<ReactDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReactDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReactDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
