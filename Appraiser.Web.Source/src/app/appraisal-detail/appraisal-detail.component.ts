import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute, Router, NavigationEnd } from '@angular/router';
import { Location } from '@angular/common';

import { AppraisalsService, AppraisalDTO } from '../generated-services';
import { throwError } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { YearPromptComponent } from '../popups/year-prompt-component';
import { MessageboxComponent } from '../popups/messagebox-component';

@Component({
  selector: 'app-appraisal-detail',
  templateUrl: './appraisal-detail.component.html',
  styleUrls: ['./appraisal-detail.component.css']
})

export class AppraisalDetailComponent implements OnInit {

  @Input() appraisal: AppraisalDTO;

  navigationSubscription: any;

  dtPeriodStart: any;
  dtPeriodEnd: any;

  constructor(
    public dialog: MatDialog,
    private router: Router,
    private route: ActivatedRoute,
    private service: AppraisalsService,
    private location: Location) {

    // since you can navigate from /detail/5 to /detail/10 we need to reload on router movements - this does that
    this.navigationSubscription = this.router.events.subscribe(e => this.startLoad(e));
  }

  ngOnInit() { }

  startLoad(e: any): void {
    if (e instanceof NavigationEnd) {
      this.getAppraisal();
    }
  }

  getAppraisal(): void {
    const id = +this.route.snapshot.paramMap.get('id');
    this.service.getAppraisal(id)
      .subscribe(
        result => this.appraisal = result,
        error => this.handleError(error)
      );
  }

  goBack(): void {
    this.location.back();
  }

  save(title?: string, message?: string): void {
    this.service.putAppraisal(this.appraisal.id, this.appraisal)
      .subscribe(
        () => {
          this.dialog.open(MessageboxComponent, {
            width: '340px',
            data: {
              title: title || 'Success!',
              message: message || 'Your appraisal was saved successfully.'
            }
          });
        },
        error => this.handleError(error)
      );
  }

  print(): void {
    window.open('http://servername:7823/printable/' + this.appraisal.id, "_self");
  }

  lock(): void { this.appraisal.objectivesLocked = true; this.save('Success!', 'You have successfully locked this appraisal.'); }
  unlock(): void { this.appraisal.objectivesLocked = false; this.save('Success!', 'You have successfully unlocked this appraisal.'); }

  clone(): void {

    var staffSelectionOptions = (this.appraisal.currentUserIsManager) ? [this.appraisal.staff, this.appraisal.staff.manager] : [this.appraisal.staff];

    const dialogRef = this.dialog.open(YearPromptComponent, {
      width: '340px',
      data: {
        mode: 'Clone',
        title: 'What year do you want to clone into?',
        year: this.appraisal.periodStart.getFullYear() + 1,
        employee: this.appraisal.staff.id,
        employees: staffSelectionOptions
      }
    });

    dialogRef.afterClosed().subscribe(dialogResult => {
      if (dialogResult && dialogResult.year) {
        this.service.cloneAppraisal(this.appraisal.id, dialogResult.year)
          .subscribe(
            cloneResult => {
              this.router.navigate(['../detail', cloneResult.id])
            },
            error => this.handleError(error)
          );
      }
    });

  }

  handleError(error) {

    let errorMessage = '';

    if (error.error instanceof ErrorEvent) {
      errorMessage = `Error: ${error.error.message}`; // client-side error
    } else {
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`; // server-side error
    }

    this.dialog.open(MessageboxComponent, {
      width: '340px',
      data: {
        title: 'Error :(',
        message: errorMessage
      }
    });

    return throwError(errorMessage);
  }

}
