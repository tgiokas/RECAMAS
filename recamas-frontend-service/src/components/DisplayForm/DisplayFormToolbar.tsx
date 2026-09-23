import Button from '../Button/Button';
import styles from './DisplayForm.module.css';

export interface DisplayFormToolbarProps {
  isEditing: boolean;
  onEdit: () => void;
  onCancel: () => void;
  onSave: () => void;
}

export function DisplayFormToolbar({
  isEditing,
  onEdit,
  onCancel,
  onSave,
}: DisplayFormToolbarProps) {
  return (
    <div className={styles.toolbar} style={{ marginLeft: 'auto' }}>
      {!isEditing && (
        <Button
          type="button"
          stylingMode="text"
          icon="edit"
          className={`${styles.actionButton} ${styles['actionButton--icon']}`}
          onClick={onEdit}
          title="Edit"
          aria-label="Edit"
        />
      )}
      {isEditing && (
        <>
          <Button
            type="button"
            stylingMode="text"
            icon="save"
            className={`${styles.actionButton} ${styles['actionButton--icon']}`}
            onClick={onSave}
            title="Save"
            aria-label="Save"
          />
          <Button
            type="button"
            stylingMode="text"
            icon="revert"
            className={`${styles.actionButton} ${styles['actionButton--icon']}`}
            onClick={onCancel}
            title="Cancel"
            aria-label="Cancel"
          />
        </>
      )}
    </div>
  );
}
