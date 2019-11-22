import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent }   from './dashboard/dashboard.component';
import { StaffComponent } from './staff/staff.component';
import { AppraisalDetailComponent } from './appraisal-detail/appraisal-detail.component';
import { AppraisalComponent } from './appraisal/appraisal.component';
import { ManagementComponent } from './management/management.component';
import { DeleteAppraisalComponent } from './delete-appraisal/delete-appraisal.component';
import { ResponsibilitiesComponent } from './responsibilities/responsibilities.component';

const routes: Routes = [
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'staff', component: StaffComponent },
  { path: 'appraisal', component: AppraisalComponent },
  { path: 'management', component: ManagementComponent },
  { path: 'responsibilities', component: ResponsibilitiesComponent },
  { path: 'detail/:id', component: AppraisalDetailComponent },
  { path: 'management/delete/:id', component: DeleteAppraisalComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class AppRoutingModule { }