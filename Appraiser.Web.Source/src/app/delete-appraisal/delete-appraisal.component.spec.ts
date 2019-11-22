import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DeleteAppraisalComponent } from './delete-appraisal.component';

describe('DeleteAppraisalComponent', () => {
  let component: DeleteAppraisalComponent;
  let fixture: ComponentFixture<DeleteAppraisalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DeleteAppraisalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DeleteAppraisalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
