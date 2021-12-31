import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FilterableContainerComponent } from './filterable-container.component';

describe('FilterableContainerComponent', () => {
  let component: FilterableContainerComponent;
  let fixture: ComponentFixture<FilterableContainerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FilterableContainerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FilterableContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
