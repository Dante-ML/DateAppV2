import { Component, inject, OnInit } from '@angular/core';
import { MembersService } from '../../_services/members.service';
import { Member } from '../../_models/member';
import { MemberCardComponent } from '../member-card/member-card.component';
import { FormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';

@Component({
  selector: 'app-member-list',
  standalone: true,
  imports: [MemberCardComponent, PaginationModule, FormsModule],
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.css'
})

export class MemberListComponent implements OnInit {
  private accountService = inject(AccountService);
  membersService = inject(MembersService);
  userParams = new UserParams(this.accountService.currentUser());
  genderList = [{value: "female", display: "Females"}, {value: "male", display: "Males"}]

  ngOnInit(): void {
    if (!this.membersService.paginatedResult()) {
      this.loadMembers();
    }
  }

  resetFilters() {
    this.userParams = new UserParams(this.accountService.currentUser());
    this.loadMembers();
  }

  loadMembers() {
    this.membersService.getMembers(this.userParams);
  }

  pageChanged(event: any) {
    if (this.userParams.pageNumber !== event.page) {
      this.userParams.pageNumber = event.page;
      this.loadMembers();
    }
  }
}