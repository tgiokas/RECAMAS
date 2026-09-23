import { Container } from '@/components/Containers/Container/Container';
import type { DisplayFormHandle } from '@/components/DisplayForm/DisplayForm';
import { DisplayForm } from '@/components/DisplayForm/DisplayForm';
import { DisplayFormToolbar } from '@/components/DisplayForm/DisplayFormToolbar';
import { useRef, useState } from 'react';
import { personalInfoGroups } from '../dummyData';

export default function OverviewTab() {
  const displayFormRef = useRef<DisplayFormHandle>(null);
  const [isEditing, setIsEditing] = useState(false);

  return (
    <Container>
      <div style={{ display: 'flex', alignItems: 'center', gap: 8, marginBottom: 8 }}>
        <h3 style={{ margin: 0, fontSize: 16, fontWeight: 600 }}>Personal Information</h3>

        <DisplayFormToolbar
          isEditing={isEditing}
          onEdit={() => setIsEditing(true)}
          onCancel={() => setIsEditing(false)}
          onSave={() => displayFormRef.current?.save()}
        />
      </div>

      <DisplayForm
        ref={displayFormRef}
        groups={personalInfoGroups}
        isEditing={isEditing}
        onEditingChange={setIsEditing}
      />
    </Container>
  );
}
