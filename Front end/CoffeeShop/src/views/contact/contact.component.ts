import { Component, ViewEncapsulation } from '@angular/core';
import { NavbarComponent } from "../../layout/navbar/navbar.component";
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDivider } from '@angular/material/divider';
import { MatIcon } from '@angular/material/icon';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import 'ckeditor5/ckeditor5.css';
import {
	ClassicEditor,
	AccessibilityHelp,
	Alignment,
	Autoformat,
	AutoImage,
	AutoLink,
	Autosave,
	Base64UploadAdapter,
	BlockQuote,
	Bold,
	Code,
	CodeBlock,
	Essentials,
	FontBackgroundColor,
	FontColor,
	FontFamily,
	FontSize,
	FullPage,
	GeneralHtmlSupport,
	Heading,
	Highlight,
	HorizontalLine,
	HtmlComment,
	HtmlEmbed,
	ImageBlock,
	ImageCaption,
	ImageInline,
	ImageInsert,
	ImageInsertViaUrl,
	ImageResize,
	ImageStyle,
	ImageTextAlternative,
	ImageToolbar,
	ImageUpload,
	Indent,
	IndentBlock,
	Italic,
	Link,
	LinkImage,
	List,
	ListProperties,
	PageBreak,
	Paragraph,
	PasteFromOffice,
	RemoveFormat,
	SelectAll,
	ShowBlocks,
	SourceEditing,
	SpecialCharacters,
	SpecialCharactersArrows,
	SpecialCharactersCurrency,
	SpecialCharactersEssentials,
	SpecialCharactersLatin,
	SpecialCharactersMathematical,
	SpecialCharactersText,
	Strikethrough,
	Style,
	Subscript,
	Superscript,
	Table,
	TableCaption,
	TableCellProperties,
	TableColumnResize,
	TableProperties,
	TableToolbar,
	TextTransformation,
	TodoList,
	Underline,
	Undo,
	type EditorConfig
} from 'ckeditor5';
import { ProblemType } from '../../Interfaces/problemType';
import { ApiService } from '../../Api/api.service';
import { MatSelectModule } from '@angular/material/select';
import { CreateContact } from '../../Interfaces/createContact';
import { AuthService } from '../../service/auth.service';
import { User } from '../../Interfaces/user';
@Component({
	selector: 'app-feedback',
	standalone: true,
	imports: [
		NavbarComponent,
		MatButtonModule,
		MatCardModule,
		MatInputModule,
		MatFormFieldModule,
		ReactiveFormsModule,
		FormsModule,
		MatDivider,
		MatIcon,
		CKEditorModule,
		MatSelectModule,
	],
	templateUrl: './contact.component.html',
	styleUrl: './contact.component.scss',
	encapsulation: ViewEncapsulation.None,

})
export class ContactComponent {
	user: User|null = null;
	applyForm = new FormGroup({
		email: new FormControl({ value: '', disabled: true },),
		problemType: new FormControl('',[Validators.required]),
		subject: new FormControl('',[Validators.required, Validators.maxLength(255)]),
		content: new FormControl('',[Validators.required,Validators.maxLength(1000)])
	  })
	listProblemType: ProblemType[] = [];
	public Editor = ClassicEditor;
	public config: EditorConfig = {
		toolbar: {
			items: [
				'undo',
				'redo',
				'|',
				'sourceEditing',
				'showBlocks',
				'selectAll',
				'|',
				'heading',
				'style',
				'|',
				'fontSize',
				'fontFamily',
				'fontColor',
				'fontBackgroundColor',
				'|',
				'bold',
				'italic',
				'underline',
				'strikethrough',
				'subscript',
				'superscript',
				'code',
				'removeFormat',
				'|',
				'specialCharacters',
				'horizontalLine',
				'pageBreak',
				'link',
				'insertImage',
				'insertImageViaUrl',
				'insertTable',
				'highlight',
				'blockQuote',
				'codeBlock',
				'htmlEmbed',
				'|',
				'alignment',
				'|',
				'bulletedList',
				'numberedList',
				'todoList',
				'outdent',
				'indent',
				'|',
				'accessibilityHelp'
			],
			shouldNotGroupWhenFull: false
		},
		plugins: [
			AccessibilityHelp,
			Alignment,
			Autoformat,
			AutoImage,
			AutoLink,
			Autosave,
			Base64UploadAdapter,
			BlockQuote,
			Bold,
			Code,
			CodeBlock,
			Essentials,
			FontBackgroundColor,
			FontColor,
			FontFamily,
			FontSize,
			FullPage,
			GeneralHtmlSupport,
			Heading,
			Highlight,
			HorizontalLine,
			HtmlComment,
			HtmlEmbed,
			ImageBlock,
			ImageCaption,
			ImageInline,
			ImageInsert,
			ImageInsertViaUrl,
			ImageResize,
			ImageStyle,
			ImageTextAlternative,
			ImageToolbar,
			ImageUpload,
			Indent,
			IndentBlock,
			Italic,
			Link,
			LinkImage,
			List,
			ListProperties,
			PageBreak,
			Paragraph,
			PasteFromOffice,
			RemoveFormat,
			SelectAll,
			ShowBlocks,
			SourceEditing,
			SpecialCharacters,
			SpecialCharactersArrows,
			SpecialCharactersCurrency,
			SpecialCharactersEssentials,
			SpecialCharactersLatin,
			SpecialCharactersMathematical,
			SpecialCharactersText,
			Strikethrough,
			Style,
			Subscript,
			Superscript,
			Table,
			TableCaption,
			TableCellProperties,
			TableColumnResize,
			TableProperties,
			TableToolbar,
			TextTransformation,
			TodoList,
			Underline,
			Undo
		],
		fontFamily: {
			supportAllValues: true
		},
		fontSize: {
			options: [10, 12, 14, 'default', 18, 20, 22],
			supportAllValues: true
		},
		heading: {
			options: [
				{
					model: 'paragraph',
					title: 'Paragraph',
					class: 'ck-heading_paragraph'
				},
				{
					model: 'heading1',
					view: 'h1',
					title: 'Heading 1',
					class: 'ck-heading_heading1'
				},
				{
					model: 'heading2',
					view: 'h2',
					title: 'Heading 2',
					class: 'ck-heading_heading2'
				},
				{
					model: 'heading3',
					view: 'h3',
					title: 'Heading 3',
					class: 'ck-heading_heading3'
				},
				{
					model: 'heading4',
					view: 'h4',
					title: 'Heading 4',
					class: 'ck-heading_heading4'
				},
				{
					model: 'heading5',
					view: 'h5',
					title: 'Heading 5',
					class: 'ck-heading_heading5'
				},
				{
					model: 'heading6',
					view: 'h6',
					title: 'Heading 6',
					class: 'ck-heading_heading6'
				}
			]
		},
		htmlSupport: {
			allow: [
				{
					name: /^.*$/,
					styles: true,
					attributes: true,
					classes: true
				}
			]
		},
		image: {
			toolbar: [
				'toggleImageCaption',
				'imageTextAlternative',
				'|',
				'imageStyle:inline',
				'imageStyle:wrapText',
				'imageStyle:breakText',
				'|',
				'resizeImage'
			]
		},
		initialData: '',
		link: {
			addTargetToExternalLinks: true,
			defaultProtocol: 'https://',
			decorators: {
				toggleDownloadable: {
					mode: 'manual',
					label: 'Downloadable',
					attributes: {
						download: 'file'
					}
				}
			}
		},
		list: {
			properties: {
				styles: true,
				startIndex: true,
				reversed: true
			}
		},
		menuBar: {
			isVisible: true
		},
		placeholder: 'Type or paste your content here!',
		style: {
			definitions: [
				{
					name: 'Article category',
					element: 'h3',
					classes: ['category']
				},
				{
					name: 'Title',
					element: 'h2',
					classes: ['document-title']
				},
				{
					name: 'Subtitle',
					element: 'h3',
					classes: ['document-subtitle']
				},
				{
					name: 'Info box',
					element: 'p',
					classes: ['info-box']
				},
				{
					name: 'Side quote',
					element: 'blockquote',
					classes: ['side-quote']
				},
				{
					name: 'Marker',
					element: 'span',
					classes: ['marker']
				},
				{
					name: 'Spoiler',
					element: 'span',
					classes: ['spoiler']
				},
				{
					name: 'Code (dark)',
					element: 'pre',
					classes: ['fancy-code', 'fancy-code-dark']
				},
				{
					name: 'Code (bright)',
					element: 'pre',
					classes: ['fancy-code', 'fancy-code-bright']
				}
			]
		},
		table: {
			contentToolbar: ['tableColumn', 'tableRow', 'mergeTableCells', 'tableProperties', 'tableCellProperties']
		}
	}; // CKEditor needs the DOM tree before calculating the configuration.
	constructor( private apiService: ApiService) {
		this.loadProblemType();
		this.applyForm.controls['email'].setValue( localStorage.getItem('email'));
	}

	loadProblemType() {
		this.apiService.getAllProblemTypes().subscribe(
			(data: ProblemType[]) => {
				this.listProblemType = data;
				if (this.listProblemType.length > 0) {
					this.applyForm.controls['problemType'].setValue(this.listProblemType[0].problemName);
				  }
			},
			(error) => {
				console.error('Error fetching problem types:', error);
			}
		);
	}

	onSubmit(){
		let contentFormat = this.applyForm.value.content?? ''.replace(/"/g, '\\"');
		let contact: CreateContact = {
			email: localStorage.getItem('email')!,
			sendDate: new Date(),
			subject: this.applyForm.value.subject!,
			problemName: this.applyForm.value.problemType!,
			content: this.applyForm.value.content!
		  };
		  
		this.apiService.postContact(contact).subscribe( res =>{

		});
	}

	onReset() {
		this.applyForm.reset();
	  }
}
