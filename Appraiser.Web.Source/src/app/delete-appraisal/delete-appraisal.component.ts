import { Component, OnInit, ViewChild } from '@angular/core';
import { AppraisalDetailComponent } from '../appraisal-detail/appraisal-detail.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-delete-appraisal',
  templateUrl: './delete-appraisal.component.html',
  styleUrls: ['./delete-appraisal.component.css']
})
export class DeleteAppraisalComponent implements OnInit {

  @ViewChild('appraisalDetail', { static: false }) appraisalDetail: AppraisalDetailComponent;

  constructor(private router: Router) {

  }

  ngOnInit() {
  }

  delete(): void {
    this.appraisalDetail.appraisal.deleted = true;
    this.appraisalDetail.save('Delete Successful!', 'The appraisal has been deleted.');

    this.router.navigate(['./management']);
  }

}
