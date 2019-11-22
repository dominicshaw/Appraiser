import { Component } from '@angular/core';
import { AppraisalDTO, ReviewDTO, StaffService, AppraisalsService, StaffDTO } from './generated-services';
import { YearPromptComponent } from './popups/year-prompt-component';
import { Router, ActivatedRoute } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { throwError } from 'rxjs';
import { MessageboxComponent } from './popups/messagebox-component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent {

  title = 'Appraiser 1.0';

  constructor(
    public dialog: MatDialog,
    private router: Router,
    private route: ActivatedRoute,
    private staffService: StaffService,
    private appraisalService: AppraisalsService) {
  }

  add(): void {

    var staffDropDown: StaffDTO[];

    this.staffService.getMyStaff()
      .subscribe(

        result => staffDropDown = result,
        error => this.handleError(error),

        () => {

          if (staffDropDown && staffDropDown.length == 0)
            return;

          const dialogRef = this.dialog.open(YearPromptComponent, {
            width: '340px',
            data: {
              mode: 'New',
              title: 'Enter the employee and year for your new Appraisal:',
              year: new Date().getFullYear() + 1,
              employee: staffDropDown[0].id,
              employees: staffDropDown
            }
          });

          dialogRef.afterClosed().subscribe(dialogResult => {
            if (dialogResult && dialogResult.year) {

              var appraisal = new AppraisalDTO();

              appraisal.midYearReview = new ReviewDTO();
              appraisal.fullYearReview = new ReviewDTO();
              appraisal.periodStart = new Date(dialogResult.year, 0, 1);
              appraisal.periodEnd = new Date(dialogResult.year, 11, 31);              
              appraisal.staffId = dialogResult.employee;

              this.appraisalService.postAppraisal(appraisal)
                .subscribe(
                  postResult => {
                    this.router.navigate(['./detail', postResult.id], { relativeTo: this.route })
                  },
                  error => this.handleError(error)
                );
            }
          });

        }

      );
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
