import { CanDeactivateFn } from '@angular/router';
import { MemberEditComponent } from '../member/member-edit/member-edit.component';

export const preventUnsavedChangesGuard: CanDeactivateFn<MemberEditComponent> =  (component) => {
  if(component.editForm?.dirty){
    return confirm("Are ypu sure you want to continue? Any unsaved changes may be lost.")
  }
  
  return true;


};
