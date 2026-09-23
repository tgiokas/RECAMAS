import { forwardRef, useState, type ReactNode } from 'react';
import type { SelectionChangedEvent } from 'devextreme/ui/tabs';
import DxTabs, { type TabsRef, type ITabsOptions } from 'devextreme-react/tabs';
import styles from './Tabs.module.css';

export interface TabConfig {
  key: string;
  title: string;
  render: () => ReactNode;
}

export type TabsProps = Omit<
  ITabsOptions,
  'dataSource' | 'selectedIndex' | 'onSelectionChanged'
> & {
  tabs: TabConfig[];
};

//Αυτά μπορούν να αλλάξουν. Θα τα οριστικοποιήσουμε αργοτερα ώστε να υπάρχει παντού το ιδιο style
const DEFAULTS: Partial<ITabsOptions> = {
  stylingMode: 'primary',
  iconPosition: 'top',
};

export const Tabs = forwardRef<TabsRef, TabsProps>(({ tabs, ...props }, ref) => {
  const [selectedIndex, setSelectedIndex] = useState(0);
  const activeTab = tabs[selectedIndex];

  return (
    <div className={styles.tabPanelView}>
      <DxTabs
        ref={ref}
        {...DEFAULTS}
        {...props}
        dataSource={tabs.map((tab) => ({ key: tab.key, text: tab.title }))}
        keyExpr="key"
        selectedIndex={selectedIndex}
        onSelectionChanged={(e: SelectionChangedEvent) => {
          const index = tabs.findIndex((tab) => tab.key === e.addedItems?.[0]?.key);
          if (index >= 0) setSelectedIndex(index);
        }}
      />
      <div className={styles.content}>{activeTab?.render()}</div>
    </div>
  );
});

Tabs.displayName = 'Tabs';
