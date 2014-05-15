// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

function initPermissionEditPanel(config) {

    var piListPanel = $('#permissionItemListPanel').data('sbListPanel');
    piListPanel.permissions = config.permissions;
    
    var permissionError = $('#permissionError');
    
    piListPanel.valid = true;
    piListPanel.getPermission = function(type, id, remove) {
        for (var i = 0; i < piListPanel.permissions.length; i++) {
            if (piListPanel.permissions[i].entityType == type 
                && piListPanel.permissions[i].entityId == id) {
                if (remove) {
                    return piListPanel.permissions.splice(i, 1)[0];
                }
                return piListPanel.permissions[i];
            }
        }
        return null;
    };
    piListPanel.addPermission = function(type, id, mask) {
        piListPanel.permissions.push({ entityType: type, entityId: id, mask: mask });
        piListPanel.valid = true;
        permissionError.hide();
    };
    
    piListPanel.newTemplate = $('#newTemplate').removeAttr('id').remove();
    piListPanel.editorTemplate = $('#editorTemplate').remove();
    
    
    piListPanel.initEditor = function(type, id, mask) {
        
        piListPanel.valid = false;

        piListPanel.editorTemplate.unbind('click.sbListPanel');
        
        piListPanel.editorTemplate.click(function(e) { 
            e.stopPropagation();
        });
        
        piListPanel.editorTemplate.find('td.accept a').click(function() {
            var $entityType = $('#EntityType');
            var entityType = $entityType.val();
            var entityId = $('#EntityId').val();
            
            if (!entityType || entityType <= 0 || !entityId || entityId < 0) {
                permissionError.text(config.errorRequired).show();;
                return;
            }
            
            if (piListPanel.getPermission(entityType, entityId)) {
                permissionError.text(config.errorDuplicate).show();
                return;
            }
            
            var mask = 0;
            if ($('#Access').is(':checked')) {
                mask = mask | config.access;
            }
            if ($('#Create').is(':checked')) {
                mask = mask | config.create;
            }
            if ($('#Update').is(':checked')) {
                mask = mask | config.update;
            }
            if ($('#Delete').is(':checked')) {
                mask = mask | config.del;
            }
            if ($('#Admin').is(':checked')) {
                mask = mask | config.admin;
            }

            piListPanel.addPermission(entityType, entityId, mask);

            var prev = piListPanel.editorTemplate.prev();
            var data = prev.find('td.data span');
            data.attr('data-type', entityType);
            data.attr('data-id', entityId);
            //data.attr('data-mask', mask);
            prev.find('td.type').text($entityType.find('option:selected').text());
            if (entityType == config.roleId) {
                prev.find('td.name').text($('#Role').find('option:selected').text());
            }
            else if (entityType == config.roleGroupId) {
                prev.find('td.name').text($('#RoleGroup').find('option:selected').text());
            }
            else {
                prev.find('td.name').text(entityId);
            }
            prev.find('td.config.access input').attr('checked', (mask & config.access) > 0);
            prev.find('td.config.create input').attr('checked', (mask & config.create) > 0);
            prev.find('td.config.update input').attr('checked', (mask & config.update) > 0);
            prev.find('td.config.del input').attr('checked', (mask & config.del) > 0);
            prev.find('td.config.admin input').attr('checked', (mask & config.admin) > 0);
            
            piListPanel.editorTemplate.remove(); 
            prev.show();
            piListPanel.resetAlternatingRows();

            return false; 
        });
        
        piListPanel.editorTemplate.find('td.cancel a').click(function() {
            piListPanel.editorTemplate.remove();
            if (piListPanel.current) {
                piListPanel.addPermission(piListPanel.current.entityType, piListPanel.current.entityId, piListPanel.current.mask);
                piListPanel.current = null;
                $('tbody tr:hidden', piListPanel.element).show();
            }
            else {
                $('tbody tr:first', piListPanel.element).remove();
            }
            piListPanel.valid = true;
            permissionError.hide();
            //piListPanel.gridDataBound();
            piListPanel.resetAlternatingRows();
            return false;
        });
        
        piListPanel.editorTemplate.find('#EntityType').change(function() {
            var entityId = $('#EntityId');
            var role = $('#Role').hide();
            var roleGroup = $('#RoleGroup').hide();
            if (this.value == config.roleId) {
                role.show().val(entityId.val());
            }
            else if (this.value == config.roleGroupId) {
                roleGroup.show().val(entityId.val());
            }
        });
        
        piListPanel.editorTemplate.find('#Role, #RoleGroup').change(function() {
            $('#EntityId').val(this.value);
        });
        
        $('#EntityId').val(id);
        $('#EntityType').val(type).change();
        $('#Access').attr('checked', (mask & config.access) > 0);
        $('#Create').attr('checked', (mask & config.create) > 0);
        $('#Update').attr('checked', (mask & config.update) > 0);
        $('#Delete').attr('checked', (mask & config.del) > 0);
        $('#Admin').attr('checked', (mask & config.admin) > 0);
    };
    
    piListPanel.addItem = function() {
        $('#accept').click();
        if (piListPanel.valid) {
            $('tbody', this.element).prepend(piListPanel.newTemplate.clone());
            piListPanel.selectItem($('tbody tr:first', this.element)[0]);
        }
    };
    
    piListPanel.removeItem = function(link) {
        var row = $(link).closest('tr');
        if (row.attr('id') == 'editorTemplate') {
            row = row.prev();
            piListPanel.editorTemplate.remove();
        }
        var data = row.find('td.data span');
        piListPanel.getPermission(data.attr('data-type'), data.attr('data-id'), true);
        row.remove();
        this.resetAlternatingRows();
    };
    
    piListPanel.selectItem = function(row) {
        var $row = $(row);
        if (this.id != 'editorTemplate') {
            $('#accept').click();
            if (!piListPanel.valid) {
                return;
            }
            $row.hide();
            $row.after(piListPanel.editorTemplate);
            this.gridDataBound();
            this.resetAlternatingRows();
            var data = $row.find('td.data span');
            piListPanel.current = piListPanel.getPermission(data.attr('data-type'), data.attr('data-id'), true);
            piListPanel.initEditor(data.attr('data-type'), data.attr('data-id'), piListPanel.current ? piListPanel.current.mask : 0);
        }
    };
    
    var permissionEditPanel = $('#permissionEditPanel').data('sbFormPanel');
    permissionEditPanel.submitForm = function(form) {
        var formPanel = this;
        if ($.sb.checkFormSubmission(form)) {
            var data = $(form).serializeArray();
            $.each(piListPanel.permissions, function(i, p) {
                data[data.length] = { name: 'Permissions[' + i + '].EntityType', value: p.entityType };
                data[data.length] = { name: 'Permissions[' + i + '].EntityId', value: p.entityId };
                data[data.length] = { name: 'Permissions[' + i + '].Mask', value: p.mask };
            });
            $.post(form.action, $.param(data), function(response) {
                if (formPanel.ajax) {
                    formPanel.updateContent(response, 'submit');
                }
                else {
                    $('#contentPanel').html(response);
                    $.sb.displayMessageSummaryAsModalBox();
                }
            });
        }
        return false;
    };

    $('#save', permissionEditPanel.element).click(function() {
        $('#accept').click();
        if (!piListPanel.valid) {
            return false;
        }
    });
}