import Form, { GroupItem, SimpleItem } from 'devextreme-react/form';
import type { FormRef } from 'devextreme-react/form';
import type { MutableRefObject } from 'react';
import type { TcnSearchCriteria } from '@/features/tcnSearch';
import styles from './TcnSearchForm.module.css';
import Button from '@/components/Button/Button';


type TcnSearchFormProps = {
  formRef: MutableRefObject<FormRef | null>;
  initialFormData: TcnSearchCriteria;
  isSearching: boolean;
  onSearch: () => void;
  onClear: () => void;
  dateEditorOptions: Record<string, unknown>;
  nationalityEditorOptions: Record<string, unknown>;
};

export function TcnSearchForm({
  formRef,
  initialFormData,
  isSearching,
  onSearch,
  onClear,
  dateEditorOptions,
  nationalityEditorOptions,
}: TcnSearchFormProps) {
  return (
    <div className={styles.form}>
      <Form
        ref={formRef}
        colCount={3}
        defaultFormData={{ ...initialFormData }}
        labelLocation="top"
        onEditorEnterKey={onSearch}
        showColonAfterLabel={false}
      >
        <GroupItem caption="Search criteria" colCount={4} colSpan={3}>
          <SimpleItem dataField="arc" editorType="dxTextBox" label={{ text: 'ARC' }} />
          <SimpleItem dataField="name" editorType="dxTextBox" label={{ text: 'Name' }} />
          <SimpleItem dataField="surname" editorType="dxTextBox" label={{ text: 'Surname' }} />
          <SimpleItem
            dataField="nationality"
            editorOptions={nationalityEditorOptions}
            editorType="dxSelectBox"
            label={{ text: 'Nationality' }}
          />
          <SimpleItem dataField="passportNo" editorType="dxTextBox" label={{ text: 'Passport No.' }} />
          <SimpleItem
            dataField="dateOfBirth"
            editorOptions={dateEditorOptions}
            editorType="dxDateBox"
            label={{ text: 'Date of Birth' }}
          />
          <SimpleItem dataField="mdFileNumber" editorType="dxTextBox" label={{ text: 'MD File Number' }} />
        </GroupItem>
      </Form>

      <div className={styles.actions}>
        <Button text="Search" disabled={isSearching} icon="search" stylingMode='contained' onClick={onSearch} />
        <Button text="Clear" disabled={isSearching} icon="clear" stylingMode="outlined" onClick={onClear}/>
      </div>
    </div>
  );
}
