import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PostularDialogComponent } from './postular-dialog.component';

describe('PostularDialogComponent', () => {
  let component: PostularDialogComponent;
  let fixture: ComponentFixture<PostularDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PostularDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PostularDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
