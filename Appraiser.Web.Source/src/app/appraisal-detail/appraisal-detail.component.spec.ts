import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AppraisalDetailComponent } from './appraisal-detail.component';

describe('AppraisalDetailComponent', () => {
  let component: AppraisalDetailComponent;
  let fixture: ComponentFixture<AppraisalDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AppraisalDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AppraisalDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
