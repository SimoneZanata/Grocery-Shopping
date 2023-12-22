import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Item } from 'src/app/models/Item';
import { AccountService } from 'src/app/services/account.service';
import { UsersService } from 'src/app/services/users.service';

@Component({
  selector: 'app-list-items',
  templateUrl: './list-items.component.html',
  styleUrls: ['./list-items.component.scss']
})
export class ListItemsComponent {
  isEditMode: boolean = false;
  item: Item = {} as Item;
  items: Item[] = [];
  userId: number = 0;

  constructor(private userService: UsersService, private accountService: AccountService
    , private router: Router) { }

  ngOnInit(): void {
    this.getItems();
  }

  getItems() {
    this.userId = this.accountService.getUserId();
    this.userService.getItemsfromUser(this.userId).subscribe({
      next: (response) => {
        this.items = response;
      },
      error: (error) => {
        console.error(error);
      }
    });
  }

  onEdit(itemId: number) {
    this.router.navigateByUrl(`/edit/${itemId}`);
  }

  onCheckboxChange(item :Item) {
    this.item.purchased = !this.item.purchased;
    if (this.item.purchased) {
      this.userService.editItemForUser(this.userId,item.id,this.item).subscribe({
        next: (response) => {
          response=item;
          console.log(item);
        },
        error: (error) => {
          console.error(error);
        }
      });
    }
  }


  onDelete(itemId: number) {
    this.userService.deleteItemfromUser(this.userId, itemId).subscribe
      ({
        next: () => {
          console.log("item deleted");
          this.getItems();
        },
        error: (error) => {
          console.error(error);
        }
      });
  }

  addItem() {
    this.router.navigateByUrl(`add`);
  }

}






