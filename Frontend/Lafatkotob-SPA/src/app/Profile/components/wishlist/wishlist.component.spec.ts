import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WhisllistComponent } from './wishlist.component';

describe('WhisllistComponent', () => {
  let component: WhisllistComponent;
  let fixture: ComponentFixture<WhisllistComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WhisllistComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(WhisllistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
