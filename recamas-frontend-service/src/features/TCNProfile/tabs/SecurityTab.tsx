import { Container } from '@/components/Containers/Container/Container';
import type { DisplayFormHandle } from '@/components/DisplayForm/DisplayForm';
import { DisplayForm } from '@/components/DisplayForm/DisplayForm';
import { DisplayFormToolbar } from '@/components/DisplayForm/DisplayFormToolbar';
import { useRef, useState } from 'react';
import { securityTabGroups } from '../dummyData';

export default function SecurityTab() {
  const displayFormRef = useRef<DisplayFormHandle>(null);
  const [isEditing, setIsEditing] = useState(false);
  const fieldCount = securityTabGroups.reduce((sum, group) => sum + group.fields.length, 0);

  return (
    <Container>
      <div style={{ display: 'flex', alignItems: 'center', gap: 8, marginBottom: 8 }}>
        <h3 style={{ margin: 0, fontSize: 16, fontWeight: 600 }}>Security & arrival history</h3>
        <span style={{ marginLeft: 'auto', fontSize: 12 }}>
          {fieldCount} FIELDS · {securityTabGroups.length} SOURCES
        </span>
        <DisplayFormToolbar
          isEditing={isEditing}
          onEdit={() => setIsEditing(true)}
          onCancel={() => setIsEditing(false)}
          onSave={() => displayFormRef.current?.save()}
        />
      </div>

      <DisplayForm
        ref={displayFormRef}
        groups={securityTabGroups}
        isEditing={isEditing}
        onEditingChange={setIsEditing}
      />
    </Container>
  );
}
